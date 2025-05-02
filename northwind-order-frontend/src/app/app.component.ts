import { Component } from '@angular/core';
import { OrderFormComponent } from './order-form/order-form.component';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [OrderFormComponent],
  template: `<app-order-form></app-order-form>`
})
export class AppComponent {}
