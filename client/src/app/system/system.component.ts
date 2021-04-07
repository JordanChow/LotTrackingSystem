import { Component, OnInit, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { Lot } from '../_model/lot';
import { Message } from '../_model/message';
import { Plant } from '../_model/plant';
import { InventoryService } from '../_services/inventory.service';
import { LotService } from '../_services/lot.service';
import { PlantService } from '../_services/plant.service';
import { InventoryComponent } from './inventory/inventory.component';
import { LogComponent } from './log/log.component';

@Component({
  selector: 'app-system',
  templateUrl: './system.component.html',
  styleUrls: ['./system.component.css']
})

export class SystemComponent implements OnInit {
  @ViewChild(InventoryComponent) childInventory: InventoryComponent;
  @ViewChild(LogComponent) childLog: LogComponent;

  lotForm: FormGroup;
  plants = new Array<Plant>(10);
  message: Message;
  plantsLoaded = false;
  resetButton: boolean;

  constructor(private lotService: LotService, private fb: FormBuilder, private toastr: ToastrService,
              private plantService: PlantService, private inventoryService: InventoryService) { }

  // Initialize form and plant cards to display
  ngOnInit(): void {
    this.initializeForm();
    this.initializePlants();
  }

  // Initialize form and validators
  initializeForm() {
    this.lotForm = this.fb.group({
      supplier: ['', [Validators.required, Validators.maxLength(16)]],
      userLot: ['', [Validators.required, Validators.pattern('^[0-9]*$'), Validators.maxLength(5) ]],
      waferAmount: ['', [Validators.required, Validators.pattern('^[0-9]*$'), Validators.maxLength(4)]]
    });
  }

  // Initialize plants from database
  async initializePlants() {
    for (var i = 0; i < this.plants.length; i++) {
      const promiseLot = await this.convertObservableToPromise(i);

      // lot has not already been created, -1 represents a new temporary lot
      if (promiseLot.userLot === -1) {
        const tempPlant = {
          location: i + 1,
          state: 'PROCESS',
          disable: true
        };
        // Add plant to plants array
        this.plants[i] = tempPlant;
      }

      // Check the current state of the plants
      else {
        if (promiseLot.state === 'PROCESS' || promiseLot.state === 'PROCESSING') {
          var lotState = 'PROCESS';
        }
        else {
          lotState = promiseLot.state;
        }

        // Updates plant from database
        const oldPlant = {
          location: promiseLot.plantId,
          lot: promiseLot,
          state: lotState,
          disable: false
        };
        this.plants[i] = oldPlant;
      }
    }

    // Plants have completed loading
    this.plantsLoaded = true;
  }

  // Helper function - convert observable of Lot to promise to allow usage to initialize plants
  private async convertObservableToPromise(index: number): Promise<Lot>{
    return this.plantService.getLotData(index + 1).toPromise();
  }

  // Create a lot method
  create() {
    this.lotService.create(this.lotForm.value).subscribe((response) => {

      this.toastr.success('Lot created successfully');

      // Log message, lot created
      const logMessage = {
        message: `Lot: ${this.lotForm.value.userLot} created`,
        date: Date()
      };

      // Update log with message
      this.childLog.updateLog(logMessage);

      // Add created lot to queue
      this.inventoryService.addInventory(response).subscribe(() => {

        // Update queue
        this.childInventory.getLotDataFromInventory();

        // Log message moved lot to queue
        const logMessage =
        {
          message: `Lot: ${response.userLot} moved to queue`,
          date: Date()
        };

        // Update log for display
        this.childLog.updateLog(logMessage);

      }, error => this.toastr.error(error.error));

      // Reset form upon submission
      this.lotForm.reset();
    }, error => this.toastr.error(error.error));
  }

  lastPlantFinished(event: Plant) {
    if (event.state === 'COMPLETE') {

      // Lot completed message
      var logMessage =
        {
          message: `Lot: ${event.lot.userLot} completed`,
          date: Date()
        };

      // Update log
      this.childLog.updateLog(logMessage);

      // Delete lot from database
      this.lotService.delete(event.lot).subscribe(response => {

        // Reset plant array
        if (response) {
          this.plants[event.location - 1].lot = null;

          this.plants[event.location - 1].disable = true;

          this.plants[event.location - 1].state = 'PROCESS';
        }
      }, error => this.toastr.error(error.error));
    }
  }

  // Process lot
  processMode(event: Plant) {
    if (event.state === 'PROCESS' && event.disable === false) {
      event.lot.state = 'PROCESSING';

      // Add log message on processing completion
      var logMessage =
      {
        message: `Lot: ${event.lot.userLot} processing at Plant ${event.location}`,
        date: Date()
      };

      // Update log that processing is complete
      this.childLog.updateLog(logMessage);

      // Update lot state in database
      this.plantService.updateLotState(event.lot);

      // Update plants array, old location
      this.plants[event.lot.plantId - 1].state = event.lot.state;
      this.plants[event.lot.plantId - 1].disable = true;

      // Process lot
      this.lotService.process(event.lot).subscribe(() => {

        // Add log message on processing completion
        var logMessage =
        {
          message: `Lot: ${event.lot.userLot} processed at Plant ${event.location}`,
          date: Date()
        };

        // Update log that processing is complete
        this.childLog.updateLog(logMessage);

        // Check if last lot
        if (event.lot.plantId === 10) {
          
          // Set lot status
          this.plants[event.lot.plantId - 1].state = 'COMPLETE';
          this.plants[event.lot.plantId - 1].disable = false;
        }

        // Not last lot
        else {
          event.lot.state = 'MOVE';

          this.plantService.updateLotState(event.lot);

          // Update plants array
          this.plants[event.lot.plantId - 1].state = event.lot.state;
          this.plants[event.lot.plantId - 1].disable = false;
        }
      }, error => this.toastr.error(error));
    }

    // Move plant after processing
    else if (event.lot.plantId !== 10) {
      this.move(event.lot);
 }
}

  // Move current lot to next plant location
  move(currentLot: Lot) {
    this.lotService.move(currentLot).subscribe(() => {

      // Sets old spot to normal
      this.plants[currentLot.plantId - 1].state = 'PROCESS';
      this.plants[currentLot.plantId - 1].disable = true;

      // Sets new spot to ready
      this.plants[currentLot.plantId].state = 'PROCESS';
      this.plants[currentLot.plantId].disable = false;

      // Update new plant location
      this.getLotData(currentLot.plantId + 1);

      const logMessage =
        {
          message: `Lot: ${currentLot.userLot} moved to Plant ${currentLot.plantId + 1}`,
          date: Date()
        };

      this.childLog.updateLog(logMessage);

      // Reset old lot in array
      this.plants[currentLot.plantId - 1].lot = null;

    }, error => this.toastr.error(error.error));
  }

  // Get lot data from database
  getLotData(currentSpot: any) {
    this.plantService.getLotData(currentSpot).subscribe(lotData => {

      // Set new spot to database information
      this.plants[currentSpot - 1].lot = lotData;
      this.plants[currentSpot - 1].location = lotData.plantId;
    }, error => this.toastr.error(error));
  }

  // Recieve log messages and call update method
  recieveMessage($event: any)
  {
    this.childLog.updateLog($event);
  }

  // Set reset button state
  recieveLog($event: boolean)
  {
    this.resetButton = $event;
  }

  // Reset method
  reset(){
    this.plantService.clearLots().subscribe(() => {
      this.initializePlants();
      this.toastr.success('System reset successful');
    }, error => this.toastr.error(error.error));
    this.childLog.clear();
    this.childInventory.clear();
    this.resetButton = false;
  }
}