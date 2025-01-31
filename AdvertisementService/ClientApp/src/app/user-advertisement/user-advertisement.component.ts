import { Component, Inject } from '@angular/core';
import { Router } from '@angular/router';
import { AdvertisementOutputModel, HttpService } from '../http.service';
import { formatDate } from '@angular/common';

@Component({
  selector: 'app-user-advertisement',
  imports: [],
  templateUrl: './user-advertisement.component.html',
  styleUrl: './user-advertisement.component.css'
})
export class UserAdvertisementComponent {
  id: string = "";
  number: string = "";
  text: string = "";
  imageLink: string = "no-image.png";
  rating: string = "-";
  created: string = "";
  willBeDeleted: string = "";
  deleteButtonText: string = "Удалить";
  isDeleted: boolean = false;

  constructor(@Inject("USER_ADVERTISEMENT") private advertisementModel: AdvertisementOutputModel, private router: Router, private httpService: HttpService) {
    this.id = advertisementModel.id;
    this.number = advertisementModel.number.toString();
    this.text = advertisementModel.text;
    if (advertisementModel.imageLink != null)
      this.imageLink = advertisementModel.imageLink;
    this.rating = advertisementModel.rating == null ? "-" : advertisementModel.rating.toString();
    this.created = "Дата создания: " + formatDate(advertisementModel.created, "dd.MM.yyyy HH:mm", "en-US");
    this.willBeDeleted = "Дата удаления: " + formatDate(advertisementModel.willBeDeleted, "dd.MM.yyyy HH:mm", "en-US");
  }

  editButtonClick() {
    if (!this.isDeleted) {
      this.httpService.transferAdvertisementId = this.id;
      this.router.navigate(['edit-advertisement']);
    }
    else
      console.log("Error: trying edit already deleted advertisement");
  }

  deleteButtonClick() {
    if (!this.isDeleted) {
      this.httpService.deleteAdvertisement(this.id).subscribe({
        next: () => {
          this.isDeleted = true;
          this.deleteButtonText = "Удалено";
        },
        error: error => {
          console.log(error);
        }
      });
    }
  }
}
