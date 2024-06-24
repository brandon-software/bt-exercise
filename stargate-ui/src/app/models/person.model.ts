export interface Person {
    personId: number;
    name: string;
    currentRank: string;
    currentDutyTitle: string;
    careerStartDate: string | null;
    careerEndDate: string | null;
}

export interface GetPeopleResponse {
    people: Person[];
    success: boolean;
    message: string;
    responseCode: number;
}

export type GetPersonByNameData = {
    name: string;
};

export interface GetPersonByNameResponse {
    person: Person;
    success: boolean;
    message: string;
    responseCode: number;
};

export type CreatePerson = {
    name: string;
};

export type PostPersonResponse = {
    id: number;
    success: boolean;
    message: string;
    responseCode: number;
};
