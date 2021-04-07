import { DatePipe } from '@angular/common';
import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { Message } from 'src/app/_model/message';
import { LogService } from 'src/app/_services/log.service';

@Component({
  selector: 'app-log',
  templateUrl: './log.component.html',
  styleUrls: ['./log.component.css']
})

export class LogComponent implements OnInit {
  @Output() logEvent: EventEmitter<boolean> = new EventEmitter<boolean>();
  log: string[];

  constructor(private logService: LogService, private toastr: ToastrService, private datePipe: DatePipe) { }

  ngOnInit(): void {
    // Call display log on init, should be empty
    this.displayLog();
  }

  // Display log on screen
  displayLog()
  {
    this.logService.displayLog().subscribe(response => {

      // Save response in log to display
      this.log = response;
      if (this.log.length > 0) {

        /*
          Checks if there are any messages in the log, if there are emit true
          In the parent component (system) it will use the true value for the reset button state
        */
        this.logEvent.emit(true);
      }
    });
  }

  // Update log
  updateLog(input: Message)
  {
    // Add the current date time to log message
    input.date = this.datePipe.transform(input.date, 'mediumTime');

    this.logService.updateLog(input).subscribe(() => {
      // After updating log, display it
      this.displayLog();
    });
  }

  // Clear log upon reset
  clear(){
    this.logService.clearLog().subscribe(response => {
      this.log = response;
    }, error => this.toastr.error(error.error));
  }
}