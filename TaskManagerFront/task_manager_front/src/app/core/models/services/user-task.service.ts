import { inject, Injectable } from '@angular/core';
import { environment } from '../../../../environments/environment';
import { HttpClient, HttpParams } from '@angular/common/http';
import { UserTask } from '../user-task.model';
import { Observable } from 'rxjs';
import { UserTaskStatus } from '../enums/user-task-status.enum';
import { CreateUserTaskDto } from '../dtos/user-tasks/create-user-task-dto';

@Injectable({
  providedIn: 'root',
})
export class UserTaskService {
  private readonly baseUrl = `${environment.apiUrl}/tasks`;

  http = inject(HttpClient);

  getTasks(status?: UserTaskStatus): Observable<UserTask[]> {
    let params = new HttpParams();
    if (status) {
      params = params.set('status', status);
    }
    return this.http.get<UserTask[]>(this.baseUrl, { params });
  }

  
  createTask(task: CreateUserTaskDto): Observable<UserTask> {
    return this.http.post<UserTask>(this.baseUrl, task);
  }

  
  updateTaskStatus(taskId: number, status: string): Observable<void> {
    const url = `${this.baseUrl}/${taskId}/status`;
    return this.http.put<void>(url, { status });
  }

  getTasksByPriority(priority: string): Observable<UserTask[]> {
    return this.http.get<UserTask[]>(`${this.baseUrl}/priority/${priority}`);
  }
}
