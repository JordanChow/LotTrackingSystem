import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';

import { AccountService } from '../_services/account.service';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.css']
})

export class NavComponent implements OnInit {
  model: any = {};

  constructor(public accountService: AccountService, private router: Router, private toastr: ToastrService) { }

  ngOnInit(): void {
  }

  // Login method
  login() {
    this.accountService.login(this.model).subscribe(response => {

      // Save login information in model
      this.model = response;

      // Navigate to system page
      this.router.navigateByUrl('/system');
    }, error => this.toastr.error(error.error));
  }

  logout() {
    this.accountService.logout();

    // Reset input fields
    this.model = {};

    // Navigate to home page
    this.router.navigateByUrl('/');
  }
}
