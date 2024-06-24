import { Person } from "./person.model";

export type PostAstronautDutyData = {
    requestBody?: CreateAstronautDuty;
};

export type CreateAstronautDuty = {
    name?: string | null;
    rank?: string | null;
    dutyTitle?: string | null;
    dutyStartDate?: string;
};

export type GetAstronautDutyByNameData = {
    name: string;
};
export interface GetAstronautDutyByNameResponse {
    person: Person;
    success: boolean;
    message: string;
    responseCode: number;
};

export type PostAstronautDutyResponse = {
    id: number,
    success: boolean;
    message: string;
    responseCode: number;
};
