import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Component, Inject } from '@angular/core';
import { OnInit } from '@angular/core';
import { FormGroup, FormControl } from '@angular/forms';
import { ReactiveFormsModule } from '@angular/forms'; // Ensure this import is present
import { StargateService } from './../../../app/services/stargate.service';
import { CreateAstronautDuty, PostAstronautDutyResponse, PostAstronautDutyData, } from './../../models/astronautDuty.model';
import { GetPeopleResponse, Person, CreatePerson, PostPersonResponse } from './../../models/person.model';

@Component({
  selector: 'app-create-astronaut-duty',
  standalone: true,
  imports: [CommonModule, FormsModule, ReactiveFormsModule],
  templateUrl: './create-astronaut-duty.component.html',
  styleUrls: ['./create-astronaut-duty.component.css']
})

export class CreateAstronautDutyComponent implements OnInit {

  createPersonForm = new FormGroup({
    name: new FormControl(''),
  });

  astronautDutyForm = new FormGroup({
    name: new FormControl(''),
    rank: new FormControl(''),
    dutyTitle: new FormControl(''),
    dutyStartDate: new FormControl('')
  });

  _stargateService: StargateService;
  _id: number = 0;

  constructor(@Inject(StargateService) private stargateService: StargateService) {
    this._stargateService = stargateService;
  }

  ngOnInit(): void {
  }

  submitCreateAstronautDutyForm() {
    console.log(this.astronautDutyForm.value);
    const data: CreateAstronautDuty = {
      name: this.astronautDutyForm.value.name,
      rank: this.astronautDutyForm.value.rank,
      dutyTitle: this.astronautDutyForm.value.dutyTitle,
      dutyStartDate: this.astronautDutyForm.value.dutyStartDate ?? undefined
    };

    this._stargateService.callPostAstronautDuty(data).then(response => {
      const responseObj = response as PostAstronautDutyResponse;

      if (responseObj && responseObj.id) {
        this._id = responseObj.id;
        console.log('Persons set successfully:', this._id);
      } else {
        console.error('Invalid response structure:', response);
        this._id = 0;
      }
    }).catch(error => {
      console.error('Error fetching persons:', error);
      this._id = 0;
    });
  }

  submitCreatePersonForm() {
    console.log(this.createPersonForm.value);
    const data: CreatePerson = {
      name: this.createPersonForm.value.name || "",
    };

    this._stargateService.callPostPerson(data).then(response => {
      const responseObj = response as PostPersonResponse;

      if (responseObj && responseObj.id) {
        this._id = responseObj.id;
        console.log('Persons created successfully:', this._id);
      } else {
        console.error('Invalid response structure:', response);
        this._id = 0;
      }
    }).catch(error => {
      console.error('Error creating person:', error);
      this._id = 0;
    });
  }

}