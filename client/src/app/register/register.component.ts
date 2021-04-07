import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { AccountService } from '../_services/account.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})

export class RegisterComponent implements OnInit {
  @Output() cancelRegister = new EventEmitter();
  model: any = {};
  registerForm: FormGroup;

  constructor(private accountService: AccountService, private router: Router,
    private fb: FormBuilder, private toastr: ToastrService) { }

  ngOnInit(): void {
    this.initializeForm();
  }

  // Set validators for username and password
  initializeForm(){
    this.registerForm = this.fb.group({
      username: ['', Validators.required],
      password: ['', [Validators.required, Validators.minLength(4)]]
    });
  }

  // Register a new user
  register(){
    this.accountService.register(this.registerForm.value).subscribe(() => {
      this.router.navigateByUrl('/system');

      this.toastr.success('Account created successfully');

      this.cancel();
    }, error => this.toastr.error(error.error));
  }

  // Cancel method from register
  cancel(){
    this.cancelRegister.emit(false);
  }
}