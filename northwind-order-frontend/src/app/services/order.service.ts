import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class OrderService {
  private apiBase = 'https://localhost:7093/api';

  constructor(private http: HttpClient) {}

  getOrderById(id: number): Observable<any> {
    return this.http.get<any>(`${this.apiBase}/orders/${id}`);
  }
  
  getCustomers(): Observable<any[]> {
    return this.http.get<any[]>(`${this.apiBase}/customers`);
  }

  getEmployees(): Observable<any[]> {
    return this.http.get<any[]>(`${this.apiBase}/employees`);
  }

  getProducts(): Observable<any[]> {
    return this.http.get<any[]>(`${this.apiBase}/products`);
  }

  saveOrder(order: any): Observable<any> {
    if (order.orderId && order.orderId > 0) {
      // Orden ya existe → actualizar
      return this.http.put<any>(`${this.apiBase}/orders/${order.orderId}`, order);
    } else {
      // Nueva orden → crear
      return this.http.post<any>(`${this.apiBase}/orders`, order);
    }
  }  

  getOrderPdfUrl(orderId: number): string {
    return `${this.apiBase}/orders/${orderId}/pdf`;
  }

  downloadAllOrdersPdf() {
    window.open('https://localhost:7093/api/orders/pdf', '_blank');
  }
  
}
