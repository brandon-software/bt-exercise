import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CreateAstronautDutyComponent } from './create-astronaut-duty.component';

describe('CreateAstronautDutyComponent', () => {
  let component: CreateAstronautDutyComponent;
  let fixture: ComponentFixture<CreateAstronautDutyComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [CreateAstronautDutyComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CreateAstronautDutyComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
