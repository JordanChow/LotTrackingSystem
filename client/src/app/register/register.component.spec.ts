import { HttpClientTestingModule } from '@angular/common/http/testing';
import { DebugElement } from '@angular/core';
import { ComponentFixture, TestBed } from '@angular/core/testing';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { By, BrowserModule } from '@angular/platform-browser';
import { RouterTestingModule } from '@angular/router/testing';
import { ToastrModule } from 'ngx-toastr';
import { RegisterComponent } from './register.component';

describe('RegisterComponent', () => {
  let component: RegisterComponent;
  let fixture: ComponentFixture<RegisterComponent>;
  let de: DebugElement;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ RegisterComponent ],
      imports: [
          FormsModule,
          BrowserModule,
          HttpClientTestingModule,
          RouterTestingModule,
          ReactiveFormsModule,
          ToastrModule.forRoot()
      ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(RegisterComponent);
    component = fixture.componentInstance;
    de = fixture.debugElement;

    fixture.detectChanges();
  });

  it('should have a h2 of `Register Now!`', () => {
    expect(de.query(By.css('h2')).nativeElement.innerText).toBe("Sign Up");
  });

  it('should call register', () => {
      spyOn(component, 'register');
      component.register();
      expect(component.register).toHaveBeenCalled();
  });

  it('should set cancel to true', () => {
    spyOn(component, 'cancel');
    const buttonElement = de.query(
      x => x.name === 'button' && x.nativeElement.textContent === 'Cancel'
    ).nativeElement;
    buttonElement.click();
    expect(component.cancel).toHaveBeenCalled();
  });

  it('form should be invalid', () => {
      component.registerForm.controls['username'].setValue('');
      component.registerForm.controls['password'].setValue('');

      expect(component.registerForm.valid).toBeFalsy();
  })

  it('form should be valid', () => {
    component.registerForm.controls['username'].setValue('validuser');
    component.registerForm.controls['password'].setValue('validpassword');

    expect(component.registerForm.valid).toBeTruthy();
  })

  it('form field should be empty', () => {
    expect(component.registerForm.controls['username'].value).toEqual('');
    expect(component.registerForm.controls['password'].value).toEqual('');
  })
});
