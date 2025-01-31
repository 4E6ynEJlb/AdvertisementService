import { ComponentFixture, TestBed } from '@angular/core/testing';

import { AdvertisementEditorComponent } from './advertisement-editor.component';

describe('AdvertisementEditorComponent', () => {
  let component: AdvertisementEditorComponent;
  let fixture: ComponentFixture<AdvertisementEditorComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [AdvertisementEditorComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(AdvertisementEditorComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
