import { bootstrapApplication } from '@angular/platform-browser';
import { AppComponent } from './app/app.component';
import { provideHttpClient } from '@angular/common/http';
import { importProvidersFrom } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { CommonModule } from '@angular/common';

bootstrapApplication(AppComponent, {
  providers: [
    provideHttpClient(), // 👈 importante para evitar NullInjectorError
    importProvidersFrom(FormsModule),
    importProvidersFrom(CommonModule)
  ]
}).catch(err => console.error(err));
