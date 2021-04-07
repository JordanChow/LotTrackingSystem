import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { Lot } from '../_model/lot';

@Injectable({
  providedIn: 'root'
})

export class InventoryService {
  baseUrl = environment.apiUrl;

  constructor(private http: HttpClient) { }

  startLotFromInventory(){
    return this.http.get<Lot>(this.baseUrl + 'inventory/start');
  }

  getLotDataFromInventory(){
    return this.http.get<Lot[]>(this.baseUrl + 'inventory/lotdata');
  }

  addInventory(lot: Lot){
    return this.http.put(this.baseUrl + 'inventory/add', lot);
  }

  deleteQueue(){
    return this.http.delete<Lot[]>(this.baseUrl + 'inventory/delete');
  }
}