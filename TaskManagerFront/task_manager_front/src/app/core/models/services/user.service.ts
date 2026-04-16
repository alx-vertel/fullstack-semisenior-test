import { inject, Injectable } from '@angular/core';
import { environment } from '../../../../environments/environment';
import { Observable } from 'rxjs';
import { User } from '../user.model';
import { HttpClient } from '@angular/common/http';
import { CreateUserDto } from '../dtos/user/create-user-dto';

@Injectable({
  providedIn: 'root',
})
export class UserService {
  private readonly baseUrl = `${environment.apiUrl}/users`;

  http = inject(HttpClient);

  getUsers(): Observable<User[]> {
    return this.http.get<User[]>(this.baseUrl);
  }

  createUser(user: CreateUserDto): Observable<User> {
    return this.http.post<User>(this.baseUrl, user);
  }
}
