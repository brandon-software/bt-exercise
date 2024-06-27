import { Injectable } from '@angular/core';
import axios from 'axios';
import { GetAstronautDutyByNameResponse, PostAstronautDutyData, CreateAstronautDuty, PostAstronautDutyResponse } from './../models/astronautDuty.model';
import { CreatePerson, PostPersonResponse, GetPeopleResponse, GetPersonByNameData, GetPersonByNameResponse } from './../models/person.model';

const BASE_URL = 'http://127.0.0.1:5205';
// const BASE_URL = 'http://localhost:5204';

async function postPerson(data: CreatePerson): Promise<PostPersonResponse> {
  try {
    // const response = await axios.post(`${BASE_URL}/person`, data);
    const payload = `"${data.name}"`;
    const response = await axios.post(`${BASE_URL}/person`, payload, {
      headers: {
        'Content-Type': 'application/json'
      }
    });
    console.log('person posted successfully');
    return response.data;

  } catch (error) {
    console.error('Error posting person:', error);
    throw error;
  }
}

async function getPersonByName(data: GetPersonByNameData): Promise<GetPersonByNameResponse> {
  try {
    const response = await axios.get<GetPersonByNameResponse>(`${BASE_URL}/Person/${data.name}`);
    return response.data;
  } catch (error) {
    console.error('Error fetching person by name:', error);
    throw error;
  }
}

async function getPeople(): Promise<GetPeopleResponse> {
  try {
    const response = await axios.get<GetPeopleResponse>(`${BASE_URL}/Person`);
    return response.data;
  } catch (error) {
    console.error('Error fetching all persons:', error);
    throw error;
  }
}
async function getAstronautDuty(data: CreatePerson): Promise<GetAstronautDutyByNameResponse> {
  try {
    const response = await axios.get<GetPersonByNameResponse>(`${BASE_URL}/Person/${data.name}`);
    console.log('get AstronautDuty successful');
    return response.data;
  } catch (error) {
    console.error('Error getting AstronautDuty:', error);
    throw error;
  }
}

async function postAstronautDuty(data: CreateAstronautDuty): Promise<PostAstronautDutyResponse> {
  try {
    const response = await axios.post(`${BASE_URL}/AstronautDuty`, data);
    console.log('Astronaut duty posted successfully');
    return response.data;

  } catch (error) {
    console.error('Error posting astronaut duty:', error);
    throw error;
  }
}

export class StargateService {

  async callPostPerson(data: CreatePerson): Promise<PostPersonResponse> {
    return await postPerson(data);
  }
  async callGetPersonByName(data: GetPersonByNameData): Promise<GetPersonByNameResponse> {
    return await getPersonByName(data);
  }
  async callGetPeople(): Promise<GetPeopleResponse> {
    return await getPeople();
  }

  async callPostAstronautDuty(data: CreateAstronautDuty): Promise<PostAstronautDutyResponse> {
    return await postAstronautDuty(data);
  }
  async callGetAstronautDuty(data: GetPersonByNameData): Promise<GetAstronautDutyByNameResponse> {
    return await getAstronautDuty(data);
  }
}