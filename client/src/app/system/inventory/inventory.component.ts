import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { Lot } from 'src/app/_model/lot';
import { Message } from 'src/app/_model/message';
import { Plant } from 'src/app/_model/plant';
import { InventoryService } from 'src/app/_services/inventory.service';

@Component({
  selector: 'app-inventory',
  templateUrl: './inventory.component.html',
  styleUrls: ['./inventory.component.css']
})

export class InventoryComponent implements OnInit {
  @Input() plants = Array<Plant>(10);
  @Output() messageEvent: EventEmitter<Message> = new EventEmitter<Message>();

  inventory = new Array<Lot>();

  constructor(private inventoryService: InventoryService, private toastr: ToastrService) { }

  ngOnInit(): void {

    // Get lot data from db on init
    this.getLotDataFromInventory();
  }

  getLotDataFromInventory(){
    this.inventoryService.getLotDataFromInventory().subscribe(response => {

      // Save inventory information from database
      this.inventory = response;
    }, error => this.toastr.error(error));
  }

  // Starting a lot from the queue
  startLotFromInventory(){
    this.inventoryService.startLotFromInventory().subscribe((response) => {
      this.toastr.success('Lot started successfully');

      // Log message of starting a lot
      const logMessage =
        {
          message: `Lot: ${response.userLot} started`,
          date: Date()
        };

      // Emit log message event to pass up to parent (system) and update log
      this.messageEvent.emit(logMessage);

      // Creating plant
      const singlePlant = {
        location: response.plantId,
        lot: response,
        state: 'PROCESS',
        disable: false
      };

      // Saving plant data in plant array
      this.plants[response.plantId - 1] = singlePlant;

      // Update plant for display
      this.getLotDataFromInventory();

    }, error => this.toastr.error(error.error));
  }

  // Clear inventory on reset
  clear(){
    this.inventoryService.deleteQueue().subscribe(response => {
      this.inventory = response;
    }, error => this.toastr.error(error.error));
  }
}