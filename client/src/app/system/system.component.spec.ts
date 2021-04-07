import { HttpClientTestingModule } from '@angular/common/http/testing';
import { DebugElement } from '@angular/core';
import { ComponentFixture, TestBed } from '@angular/core/testing';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { By, BrowserModule } from '@angular/platform-browser';
import { RouterTestingModule } from '@angular/router/testing';
import { ToastrModule } from 'ngx-toastr';
import { SystemComponent } from './system.component';

describe('SystemComponent', () => {
  let component: SystemComponent;
  let fixture: ComponentFixture<SystemComponent>;
  let de: DebugElement;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ SystemComponent ],
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
    fixture = TestBed.createComponent(SystemComponent);
    component = fixture.componentInstance;
    de = fixture.debugElement;

    fixture.detectChanges();
  });

  it('plants array should be initialized', () => {
    expect(component.plants.length).toEqual(10);
  })

  it('form field should be empty', () => {
    expect(component.lotForm.controls['supplier'].value).toEqual('');
    expect(component.lotForm.controls['waferAmount'].value).toEqual('');
    expect(component.lotForm.controls['userLot'].value).toEqual('');
  })

  it('form field should not be valid', () => {
    component.lotForm.controls['supplier'].setValue("a");
    component.lotForm.controls['waferAmount'].setValue("a");
    component.lotForm.controls['userLot'].setValue("a");

    expect(component.lotForm.valid).toBeFalsy();
  })

  it('create button should be disabled', () => {
    expect(component.lotForm.valid).toBeFalsy();
  })
  
  it('should call create', () => {
    spyOn(component, 'create');
    component.create();
    expect(component.create).toHaveBeenCalled();
  })

  it('should call process', () => {
    spyOn(component, 'processMode');
    var plant;
    component.processMode(plant);
    expect(component.processMode).toHaveBeenCalled();
  })

  it('should call move', () => {
    spyOn(component, 'move');
    var plant;
    component.move(plant);
    expect(component.move).toHaveBeenCalled();
  })
});
