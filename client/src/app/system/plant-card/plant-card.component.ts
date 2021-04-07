import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { Plant } from 'src/app/_model/plant';

@Component({
  selector: 'app-plant-card',
  templateUrl: './plant-card.component.html',
  styleUrls: ['./plant-card.component.css']
})
export class PlantCardComponent implements OnInit {
  @Input() plantFromParent: Plant;
  @Output() processLot: EventEmitter<Plant> = new EventEmitter();
  @Output() lotComplete: EventEmitter<Plant> = new EventEmitter();

  constructor() {}

  ngOnInit(): void {
  }

  // Process method when a lot is processed
  process(plant: Plant) {
    // Emit event to parent method (system) for lot processing, passes plant
    this.processLot.emit(plant);

    // Check if last plant location
    if (plant.location === 10) {

      // After processing once set state to complete
      plant.lot.state = 'COMPLETE';

      // Ensures the lot is completed processing
      if (plant.lot.state === 'COMPLETE') {
        this.lotComplete.emit(plant);
      }
    }
  }

  // Set color of button method depending on state of plant
  getColor() {
    if (this.plantFromParent.state === 'PROCESSING') { return '#FF8C00'; }

    else if (this.plantFromParent.state === 'MOVE' ||
      this.plantFromParent.state === 'COMPLETE') { return '#28a745'; }

    else if (this.plantFromParent.state === 'PROCESS') { return '#1E90FF'; }
  }
}
