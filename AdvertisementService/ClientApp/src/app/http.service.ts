import { EventEmitter, Injectable } from '@angular/core';
import { HttpClient, HttpClientModule, HttpErrorResponse, HttpHeaders } from '@angular/common/http'
import { BehaviorSubject, Observable, Subject } from 'rxjs';
import { JwtHelperService } from '@auth0/angular-jwt'
import { Router } from '@angular/router';

@Injectable({
  providedIn: 'root'
})
export class HttpService {
  private readonly domain: string = "https://localhost:12346/";
  private readonly advertisement: string = this.domain + "Advertisement/";
  private readonly auth: string = this.domain + "Auth/";
  private readonly user: string = this.domain + "User/";
  private readonly helper: JwtHelperService = new JwtHelperService();
  public isLogined: boolean = false;
  public isAdmin: boolean = false;
  private jwt: string | null = null;
  public credentials: Credentials | null = null;
  public transferAdvertisementId: string | null = null;

  public logInEvent = new EventEmitter();
  public logOutEvent = new EventEmitter();

  constructor(private client: HttpClient, private router: Router) { }

  public getAdvertisements(options: GetAdvertisementsOptions): Observable<AdvertisementsPagesOutput> {
    var headers = new HttpHeaders().set("accept", "text/plain").set("Content-Type", "application/json");
    return this.client.post<AdvertisementsPagesOutput>(this.advertisement + "GetAdvertisements", options, { headers: headers });
  }

  public login(credentials: Credentials) {
    var headers = new HttpHeaders().set("accept", "text/plain").set("Content-Type", "application/json");
    this.client.post(this.auth + "AuthUser", credentials, { headers: headers, responseType: 'text' }).subscribe(
      res => {
        this.jwt = res;
        this.credentials = credentials;
        this.isLogined = true;
        this.logInEvent.emit();
      },
      error => {
        if ((<HttpErrorResponse>error).status == 401)
          alert("Неверный логин или пароль");
        else
          console.log(error);
      }
    );
  }

  public logout() {
    this.credentials = null;
    this.jwt = null;
    this.isAdmin = false;
    this.isLogined = false;
    this.logOutEvent.emit();
  }

  public register(registerUserModel: RegisterUserModel) {
    var headers = new HttpHeaders().set("accept", "text/plain").set("Content-Type", "application/json");
    this.client.post(this.auth + "Register", registerUserModel, { headers: headers, responseType: 'text' }).subscribe(
      res => {
        this.jwt = res;
        this.credentials = new Credentials();
        this.credentials.login = registerUserModel.login;
        this.credentials.password = registerUserModel.password;
        this.isLogined = true;
        this.logInEvent.emit();
      },
      error => {
        if ((<HttpErrorResponse>error).status == 400)
          alert("Логин или пароль уже используется");
        else
          console.log(error);
      }
    );
  }

  public getUser(): Observable<UserOutputModel> {
    this.checkToken();
    var headers = new HttpHeaders().set("accept", "text/plain").set("Authorization", "Bearer " + this.jwt);
    return this.client.get<UserOutputModel>(this.user + "GetUser", { headers: headers });
  }

  public getUserAdvertisements(): Observable<AdvertisementOutputModel[]> {
    this.checkToken();
    var headers = new HttpHeaders().set("accept", "text/plain").set("Authorization", "Bearer " + this.jwt);
    return this.client.get<AdvertisementOutputModel[]>(this.advertisement + "GetUserAdvertisements", { headers: headers });
  }

  public getAdvertisement(id: string): Observable<AdvertisementOutputModel> {
    var headers = new HttpHeaders().set("accept", "text/plain");
    return this.client.get<AdvertisementOutputModel>(this.advertisement + "GetAdvertisement?id=" + id, { headers: headers });
  }

  public deleteAdvertisementAdmin(id: string): Observable<null> {
    this.checkToken();
    var headers = new HttpHeaders().set("accept", "*/*").set("Authorization", "Bearer " + this.jwt);
    return this.client.delete<null>(this.advertisement + "DeleteAdvertisementAdmin?id=" + id, { headers: headers });
  }

  public rateAdvertisement(id: string, mark: number): Observable<number> {
    this.checkToken();
    var headers = new HttpHeaders().set("accept", "text/plain").set("Authorization", "Bearer " + this.jwt);
    return this.client.patch<number>(this.advertisement + "RateAdvertisement?id=" + id + "&mark=" + mark.toString(), "", { headers: headers });
  }

  public createAdvertisement(number: number, text: string): Observable<string> {
    this.checkToken();
    var headers = new HttpHeaders().set("accept", "text/plain").set("Content-Type", "application/json").set("Authorization", "Bearer " + this.jwt);
    return this.client.post<string>(this.advertisement + "CreateAdvertisement", { number: number, text: text }, { headers: headers });
  }

  public attachImage(id: string, file: File): Observable<string> {
    this.checkToken();
    const formData = new FormData();
    formData.append("file", file, file.name);
    var headers = new HttpHeaders().set("accept", "text/plain").set("Authorization", "Bearer " + this.jwt);
    return this.client.patch<string>(this.advertisement + "AttachImage?id=" + id, formData, { headers: headers });
  }

  public editAdvertisement(id: string, number: number, text: string): Observable<null> {
    this.checkToken();
    var headers = new HttpHeaders().set("accept", "*/*").set("Content-Type", "application/json").set("Authorization", "Bearer " + this.jwt);
    return this.client.patch<null>(this.advertisement + "EditAdvertisement?id=" + id, { number: number, text: text }, { headers: headers });
  }

  public deleteImage(id: string): Observable<null> {
    this.checkToken();
    var headers = new HttpHeaders().set("accept", "*/*").set("Authorization", "Bearer " + this.jwt);
    return this.client.delete<null>(this.advertisement + "DeleteImage?id=" + id, { headers: headers });
  }

  public deleteAdvertisement(id: string): Observable<null> {
    this.checkToken();
    var headers = new HttpHeaders().set("accept", "*/*").set("Authorization", "Bearer " + this.jwt);
    return this.client.delete<null>(this.advertisement + "DeleteAdvertisement?id=" + id, { headers: headers });
  }

  public editCredentials(login: string, password: string) {
    this.checkToken();
    var headers = new HttpHeaders().set("accept", "*/*").set("Content-Type", "application/json").set("Authorization", "Bearer " + this.jwt);
    this.client.patch<null>(this.user + "EditCredentials", { login: login, password: password }, { headers: headers }).subscribe({
      next: () => {
        this.credentials = new Credentials();
        this.credentials.login = login;
        this.credentials.password = password;
        this.updateToken();
        this.router.navigate(['profile']);
      },
      error: error => {
        if ((<HttpErrorResponse>error).status == 400)
          alert("Логин или пароль уже используется");
        else
          console.log(error);
      }
    })
  }

  public editName(name: string): Observable<null> {
    this.checkToken();
    var headers = new HttpHeaders().set("accept", "*/*").set("Authorization", "Bearer " + this.jwt);
    return this.client.patch<null>(this.user + "EditName?name=" + name, "", { headers: headers });
  }

  public deleteUser(): Observable<null> {
    this.checkToken();
    var headers = new HttpHeaders().set("accept", "*/*").set("Authorization", "Bearer " + this.jwt);
    return this.client.delete<null>(this.user + "DeleteUser", { headers: headers });
  }

  private checkToken() {
    if (this.jwt != null && this.helper.isTokenExpired(this.jwt))
      this.updateToken();
  }

  private updateToken() {
    if (this.credentials == null)
      console.log("Credentials are null");
    else {
      var headers = new HttpHeaders().set("accept", "text/plain").set("Content-Type", "application/json");
      this.client.post(this.auth + "AuthUser", this.credentials, { headers: headers, responseType: 'text' }).subscribe(
        res => {
          this.jwt = res;
        },
        error => {
          console.log(error);
        }
      );
    }
  }
}

export class Credentials {
  login: string | undefined;
  password: string | undefined;
}

export class RegisterUserModel {
  login: string | undefined;
  password: string | undefined;
  name: string | undefined;
}

export class GetAdvertisementsOptions {
  criterion: number | undefined;
  isASC: boolean | undefined;
  keyWord: string | null | undefined;
  ratingLow: number | undefined;
  ratingHigh: number | undefined;
  page: number | undefined;
  pageSize: number | undefined;
}

export class AdvertisementsPagesOutput {
  advertisements: AdvertisementOutputModel[] | null = null;
  pagesCount: number = 0;
}

export class AdvertisementOutputModel {
  id: string = "";
  number: number = 0;
  userId: string = "";
  text: string = "";
  imageLink: string | null = null;
  rating: number | null = null;
  created: string = "";
  willBeDeleted: string = "";
}

export class UserOutputModel {
  id: string = "";
  name: string = "";
  isAdmin: boolean = false;
}