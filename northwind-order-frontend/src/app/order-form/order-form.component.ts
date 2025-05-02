import {
  Component,
  AfterViewInit,
  ViewChild,
  ElementRef
} from '@angular/core';
import { DomSanitizer, SafeResourceUrl } from '@angular/platform-browser';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';
import { OrderService } from '../services/order.service';

declare const google: any;

@Component({
  selector: 'app-order-form',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './order-form.component.html',
  styleUrls: ['./order-form.component.css']
})
export class OrderFormComponent implements AfterViewInit {
  @ViewChild('shippingAddressInput') shippingAddressInput!: ElementRef;

  customers: any[] = [];
  employees: any[] = [];
  productCatalog: any[] = [];

  searchOrderId: number | null = null;

  order = {
    orderId: 0,
    customerId: '',
    employeeId: 0,
    orderDate: '',
    shipAddress: '',
    shipCity: '',
    shipRegion: '',
    shipPostalCode: '',
    shipCountry: '',
    orderDetails: [] as any[]
  };

  validated = {
    coordinates: ''
  };

  newLine = {
    productId: null,
    quantity: 1
  };

  mapUrl: SafeResourceUrl = '';

  constructor(
    private sanitizer: DomSanitizer,
    private orderService: OrderService
  ) {
    this.loadDropdowns();
  }

  ngAfterViewInit(): void {
    const autocomplete = new google.maps.places.Autocomplete(
      this.shippingAddressInput.nativeElement,
      {
        types: ['geocode'],
        fields: ['address_components', 'geometry', 'formatted_address']
      }
    );

    autocomplete.addListener('place_changed', () => {
      const place = autocomplete.getPlace();
      if (!place.geometry || !place.address_components) return;

      const components: any = {};
      place.address_components.forEach((component: any) => {
        const type = component.types[0];
        components[type] = component.long_name;
      });

      this.order.shipCity = components['locality'] || '';
      this.order.shipRegion = components['administrative_area_level_1'] || '';
      this.order.shipPostalCode = components['postal_code'] || '';
      this.order.shipCountry = components['country'] || '';

      const lat = place.geometry.location.lat();
      const lng = place.geometry.location.lng();
      this.validated.coordinates = `${lat}, ${lng}`;

      this.order.shipAddress = place.formatted_address;
      this.updateMap(lat, lng);
    });
  }

  updateMap(lat: number, lng: number): void {
    const url = `https://www.google.com/maps/embed/v1/place?key=AIzaSyB1JeCzt7tzd-kXkknXKKyMVK07qA135oc&q=${lat},${lng}`;
    this.mapUrl = this.sanitizer.bypassSecurityTrustResourceUrl(url);
  }

  loadDropdowns(): void {
    this.orderService.getCustomers().subscribe(res => this.customers = res);
    this.orderService.getEmployees().subscribe(res => {
      this.employees = res;
    });
    this.orderService.getProducts().subscribe(res => this.productCatalog = res);
  }

  searchOrder(): void {
    if (!this.searchOrderId) return;

    this.orderService.getOrderById(this.searchOrderId).subscribe({
      next: (res: any) => {
        this.order = res;
        const coords = res.shipCity && res.shipCountry ? `${res.shipCity}, ${res.shipCountry}` : '';
        this.validated.coordinates = coords;
      },
      error: () => alert('Orden no encontrada')
    });
  }

  addLine(): void {
    if (this.newLine.productId == null) {
      alert('Por favor selecciona un producto');
      return;
    }

    const product = this.productCatalog.find(
      p => p.productId === +this.newLine.productId!
    );

    if (!product) {
      alert('Producto no válido');
      return;
    }

    const existing = this.order.orderDetails.find(
      od => od.productId === product.productId
    );

    if (existing) {
      existing.quantity += this.newLine.quantity;
    } else {
      this.order.orderDetails.push({
        productId: product.productId,
        productName: product.productName,
        quantity: this.newLine.quantity,
        unitPrice: product.unitPrice
      });
    }

    this.newLine = { productId: null, quantity: 1 };
  }

  removeLine(index: number): void {
    this.order.orderDetails.splice(index, 1);
  }

  newOrder() {
    this.order = {
      orderId: 0,
      customerId: '',
      employeeId: 0,
      orderDate: '',
      shipAddress: '',
      shipCity: '',
      shipRegion: '',
      shipPostalCode: '',
      shipCountry: '',
      orderDetails: []
    };
    this.validated = { coordinates: '' };
    this.newLine = { productId: null, quantity: 1 };
    this.mapUrl = '';
    this.searchOrderId = null;
  }

  saveOrder() {
    if (!this.order.customerId || !this.order.employeeId || !this.order.orderDate || this.order.orderDetails.length === 0) {
      alert('Todos los campos y al menos un producto son requeridos.');
      return;
    }

    this.orderService.saveOrder(this.order).subscribe({
      next: (res: any) => {
        alert('Orden guardada correctamente');
        this.order.orderId = res.orderId ?? 0;
      },
      error: err => alert('Error al guardar la orden')
    });
  }

  deleteOrder() {
    this.newOrder();
  }

  generatePdf() {
    if (!this.order.orderId) {
      alert('Primero guarda la orden para poder generar PDF.');
      return;
    }
    window.open(this.orderService.getOrderPdfUrl(this.order.orderId), '_blank');
  }

  // Descarga PDF con todas las órdenes
  downloadAllOrdersPdf(): void {
  window.open('https://localhost:7093/api/orders/pdf', '_blank');
  }
}
