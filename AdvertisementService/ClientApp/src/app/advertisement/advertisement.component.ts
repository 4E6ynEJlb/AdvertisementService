import { Component, Inject } from '@angular/core';
import { AdvertisementOutputModel, HttpService } from '../http.service';
import { formatDate } from '@angular/common';

@Component({
  selector: 'app-advertisement',
  imports: [],
  templateUrl: './advertisement.component.html',
  styleUrl: './advertisement.component.css'
})
export class AdvertisementComponent {
  id: string = "";
  number: string = "";
  text: string = "";
  imageLink: string = "no-image.png";
  rating: string = "-";
  created: string = "";
  stars: string[] = ["☆", "☆", "☆", "☆", "☆"];
  isRated: boolean = false;
  isDeleteButtonVisible: boolean = false;
  deleteButtonText: string = "Удалить";
  isDeleted: boolean = false;

  constructor(@Inject("ADVERTISEMENT") private advertisementModel: AdvertisementOutputModel, private httpService: HttpService) {
    this.number = advertisementModel.number.toString();
    this.text = advertisementModel.text;
    if (advertisementModel.imageLink != null)
      this.imageLink = advertisementModel.imageLink;
    this.rating = advertisementModel.rating == null ? "-" : advertisementModel.rating.toString();
    this.created = formatDate(advertisementModel.created, "dd.MM.yyyy HH:mm", "en-US");
    this.id = advertisementModel.id;
    this.isDeleteButtonVisible = httpService.isAdmin;
  }

  deleteButtonClick() {
    if (!this.isDeleted && this.isDeleteButtonVisible) {
      this.httpService.deleteAdvertisementAdmin(this.id).subscribe({
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

  starClick(mark: number) {
    if (!this.isRated && !this.isDeleted) {
      if (!this.httpService.isLogined) {
        alert("Для оценивания объявлений необходимо зарегистрироваться или войти");
        return;
      }
      this.httpService.rateAdvertisement(this.id, mark).subscribe({
        next: (newRating: number) => {
          this.rating = newRating.toString();
          this.isRated = true;
        },
        error: error => {
          console.log(error);
        }
      })
    }
  }

  starMouseEnter(starIndex: number) {
    if (!this.isRated && !this.isDeleted)
      for (var index: number = 0; index <= starIndex; index++) {
        this.stars[index] = "★"
      }
  }

  starMouseLeave() {
    if (!this.isRated && !this.isDeleted)
      for (var index: number = 0; index <= 4; index++) {
        this.stars[index] = "☆"
      }
  }
}
