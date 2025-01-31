import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AdvertisementsComponent } from './advertisements.component';

describe('AdvertisementsComponent', () => {
  let component: AdvertisementsComponent;
  let fixture: ComponentFixture<AdvertisementsComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [AdvertisementsComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AdvertisementsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
