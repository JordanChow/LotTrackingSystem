import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { map } from 'rxjs/operators';
import { environment } from 'src/environments/environment';
import { Lot } from '../_model/lot';

@Injectable({
  providedIn: 'root'
})

export class LotService {
  baseUrl = environment.apiUrl;

  constructor(private http: HttpClient, private toastr: ToastrService) { }

  // Creating a lot
  create(model: any) {
    return this.http.post(this.baseUrl + 'lot/create', model).pipe(
      map((createdLot: Lot) => {
        if (createdLot)
        {
          return createdLot;
        }
      })
    );
  }

  process(lot: Lot)
  {
    return this.http.put<boolean>(this.baseUrl + 'lot/process', lot);
  }

  move(lot: Lot)
  {
    return this.http.put<boolean>(this.baseUrl + 'lot/move', lot);
  }

  delete(lot: Lot)
  {
    return this.http.put<boolean>(this.baseUrl + 'lot/delete', lot);
  }
}