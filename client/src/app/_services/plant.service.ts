import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { Lot } from '../_model/lot';

@Injectable({
  providedIn: 'root'
})

export class PlantService {
  baseUrl = environment.apiUrl;

  constructor(private http: HttpClient) { }

  getLotData(location: number) {
    return this.http.get<Lot>(this.baseUrl + 'plant/' + location);
  }

  updateLotState(lot: Lot){
    this.http.post(this.baseUrl + 'plant/updateLot', lot);
  }

  clearLots() {
    return this.http.delete(this.baseUrl + 'plant/clear');
  }
}