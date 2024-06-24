import { Component } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { StargateService } from './services/stargate.service';
@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css',
  providers: [StargateService]
})
export class AppComponent {
  title = 'stargate-ui';
}
