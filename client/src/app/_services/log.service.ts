import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { Message } from '../_model/message';

@Injectable({
  providedIn: 'root'
})

export class LogService {
  baseUrl = environment.apiUrl;

  constructor(private http: HttpClient) { }

  updateLog(input: Message) {
    return this.http.put(this.baseUrl + 'log/update', input);
  }

  displayLog() {
    return this.http.get<string[]>(this.baseUrl + 'log/display');
  }

  clearLog() {
    return this.http.delete<string[]>(this.baseUrl + 'log/clear');
  }
}