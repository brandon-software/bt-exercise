import { Component, Inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { StargateService } from './../../../app/services/stargate.service';
import { GetPersonByNameData, GetPeopleResponse } from './../../models/person.model';
import { GetPersonByNameResponse, Person } from './../../models/person.model';
import { FormsModule } from '@angular/forms';

@Component({
  selector: 'app-people',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './people.component.html',
  styleUrls: ['./people.component.css']
})
export class PeopleComponent {
  persons: Person[] = [];
  specificPerson: Person | undefined;
  searchName: string = '';

  _stargateService: StargateService;

  constructor(@Inject(StargateService) private stargateService: StargateService) {
    this._stargateService = stargateService;
  }

  ngOnInit() {
    this.getPersons();
  }

  getPersons() {
    this._stargateService.callGetPeople().then(response => {
      const responseObj = response as GetPeopleResponse;

      if (responseObj && responseObj.people) {
        this.persons = responseObj.people;
        console.log('Persons set successfully:', this.persons);
      } else {
        console.error('Invalid response structure:', response);
        this.persons = [];
      }
    }).catch(error => {
      console.error('Error fetching persons:', error);
      this.persons = [];
    });
  }

  getPersonByName() {
    const data: GetPersonByNameData = { name: 'John Doe' };

    this._stargateService.callGetPersonByName(data).then(response => {
      const responseObj = response as GetPersonByNameResponse;

      if (responseObj && responseObj.person) {
        this.persons = [responseObj.person];
        console.log('Persons set successfully:', this.persons);
      } else {
        console.error('Invalid response structure:', response);
        this.persons = [];
      }
    }).catch(error => {
      console.error('Error fetching persons:', error);
      this.persons = [];
    });
  }
  onNameChange() {
    if (this.searchName.length > 0) {
      const data: GetPersonByNameData = { name: this.searchName };
      this._stargateService.callGetPersonByName(data).then(response => {
        const responseObj = response as GetPersonByNameResponse;

        if (responseObj && responseObj.person) {
          // set the this.persons object with and array of objects with the only person
          this.persons = [responseObj.person];
          console.log('Persons set successfully:', this.persons);
        } else {
          console.error('Invalid response structure:', response);
          this.persons = [];
        }
      }).catch(error => {
        console.error('Error fetching persons:', error);
        this.persons = [];
      });
    } else {
      this.getPersons(); // Reset to original list if search is cleared
    }
  }
  search() {
    console.log(this.searchName); // Do something with the form value
  }
}