import { Component } from '@angular/core';
import { PeopleComponent } from '../components/people/people.component';
import { CreateAstronautDutyComponent } from '../components/create-astronaut-duty/create-astronaut-duty.component';

@Component({
  selector: 'app-home',
  standalone: true,
  imports: [PeopleComponent, CreateAstronautDutyComponent],
  templateUrl: './home.component.html',
  styleUrl: './home.component.css'
})
export class HomeComponent {

}
