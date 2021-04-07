import { HttpClientTestingModule } from '@angular/common/http/testing';
import { DebugElement } from '@angular/core';
import { ComponentFixture, TestBed } from '@angular/core/testing';
import { By } from '@angular/platform-browser';
import { HomeComponent } from './home.component';

describe('HomeComponent', () => {
  let component: HomeComponent;
  let fixture: ComponentFixture<HomeComponent>;
  let de: DebugElement;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ HomeComponent ],
      imports:
      [
        HttpClientTestingModule
      ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(HomeComponent);
    component = fixture.componentInstance;
    de = fixture.debugElement;

    fixture.detectChanges();
  });

  it('should have a h3 of `Register Now!`', () => {
    expect(de.query(By.css('h3')).nativeElement.innerText).toBe("Register Now!");
  });

  it('should have register mode off on init', () => {
    expect(component.registerMode).toBeFalsy();
  });

  it('should toggle register mode', () => {
    component.registerToggle();
    expect(component.registerMode).toBeTruthy();
    component.cancelRegisterMode(true);
    expect(component.registerMode).toBeTruthy();
  });

  it('should toggle mode on button click', () => {
    spyOn(component, 'registerToggle');
    let buttonElement = de.query(By.css('button')).nativeElement;
    buttonElement.click();
    expect(component.registerToggle).toHaveBeenCalled();
  })
});
