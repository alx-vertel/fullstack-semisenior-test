import { Component, inject, OnInit, signal } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { UserTaskPriority } from '../../../../core/models/enums/user-task-priority.enum';
import { UserService } from '../../../../core/models/services/user.service';
import { UserTaskService } from '../../../../core/models/services/user-task.service';
import { Router, RouterModule } from '@angular/router';
import { User } from '../../../../core/models/user.model';
import { CreateUserTaskDto } from '../../../../core/models/dtos/user-tasks/create-user-task-dto';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-add-user-task-form.component',
  imports: [CommonModule, ReactiveFormsModule, RouterModule],
  templateUrl: './add-user-task-form.component.html',
  styleUrl: './add-user-task-form.component.css',
})
export class AddUserTaskFormComponent implements OnInit{
  private fb = inject(FormBuilder);
  private taskService = inject(UserTaskService);
  private userService = inject(UserService);
  private router = inject(Router);
  
  users = signal<User[]>([]);
  priorities = Object.values(UserTaskPriority);

  taskForm = this.fb.group({
    title: ['', [Validators.required, Validators.maxLength(100)]],
    description: [''],
    assignedUserId: ['', Validators.required],
    priority: [UserTaskPriority.Medium, Validators.required],
    tags: [''],
    estimatedEndDate: ['']
  });

  ngOnInit() {
    this.userService.getUsers().subscribe(u => this.users.set(u));
  }

  onSubmit() {
    if (this.taskForm.invalid) return;

    const formVal = this.taskForm.value;
    
    const userTaskPayload : CreateUserTaskDto = {
      title: formVal.title ?? '',
      description: formVal.description,
      userId: Number(formVal.assignedUserId),
      additionalInfo: {
        priority: formVal.priority,
        estimatedEndDate: formVal.estimatedEndDate || null, // <-- Nuevo campo
        tags: formVal.tags ? formVal.tags.split(',').map((t: string) => t.trim()) : []
      }
    };

    this.taskService.createTask(userTaskPayload).subscribe({
      next: () => this.router.navigate(['/']),
      error: () => {}
    });
  }
}
