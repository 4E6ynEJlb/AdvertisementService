import { ComponentFixture, TestBed } from '@angular/core/testing';

import { NewAdvertisementComponent } from './new-advertisement.component';

describe('NewAdvertisementComponent', () => {
  let component: NewAdvertisementComponent;
  let fixture: ComponentFixture<NewAdvertisementComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [NewAdvertisementComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(NewAdvertisementComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
