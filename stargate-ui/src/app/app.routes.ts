import { Routes } from '@angular/router';
import { PeopleComponent } from './components/people/people.component'
import { CreateAstronautDutyComponent } from './components/create-astronaut-duty/create-astronaut-duty.component'
import { HomeComponent } from './home/home.component';

export const routes: Routes = [
  { path: '', component: HomeComponent },
  { path: 'people', component: PeopleComponent },
  { path: 'duty', component: CreateAstronautDutyComponent }
];
