import { CommonModule } from '@angular/common';
import { Component, inject, OnInit } from '@angular/core';
import { Router, RouterModule } from '@angular/router';
import { UserTask } from '../../core/models/user-task.model';
import { UserTaskStatus } from '../../core/models/enums/user-task-status.enum';
import { UserTaskService } from '../../core/models/services/user-task.service';
import { signal } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { UserTaskPriority } from '../../core/models/enums/user-task-priority.enum';

@Component({
  selector: 'app-home',
  imports: [CommonModule, RouterModule, FormsModule],
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css'],
})
export class HomeComponent implements OnInit {
  stats = {
    totalTasks: 0,
    pending: 0,
    inProgress: 0,
    done: 0
  };

  selectedStatus: UserTaskStatus | '' = '';
  statuses = [
    { label: 'All', value: '' },
    { label: 'Pending', value: UserTaskStatus.Pending },
    { label: 'In Progress', value: UserTaskStatus.InProgress },
    { label: 'Done', value: UserTaskStatus.Done }
  ];

  selectedPriority: UserTaskPriority | '' = '';
  priorities = [
    { label: 'All', value: '' },
    { label: 'Low', value: UserTaskPriority.Low },
    { label: 'Medium', value: UserTaskPriority.Medium },
    { label: 'High', value: UserTaskPriority.High }
  ];

  recentTasks: UserTask[] = [];
  isLoading = signal(true);
  errorMessage = signal<string | null>(null);

  userTaskService = inject(UserTaskService);
  router = inject(Router);

  ngOnInit(): void {
    this.loadDashboardData();
  }

  loadDashboardData(
    status: UserTaskStatus | '' = this.selectedStatus,
    priority: UserTaskPriority | '' = this.selectedPriority
  ): void {
    this.isLoading.set(true);
    this.errorMessage.set(null);

    if (priority) {
      this.userTaskService.getTasksByPriority(priority).subscribe({
        next: (userTasks) => {
          const filtered = status
            ? userTasks.filter(t => t.status === status)
            : userTasks;
          this.calculateStats(filtered);
          this.recentTasks = filtered.slice(0, 5);
          this.isLoading.set(false);
        },
        error: (err) => {
          this.errorMessage.set("Data could not be loaded. Please try again.");
          this.isLoading.set(false);
        }
      });
    } else {
      this.userTaskService.getTasks(status as UserTaskStatus).subscribe({
        next: (userTasks) => {
          this.calculateStats(userTasks);
          this.recentTasks = userTasks.slice(0, 5);
          this.isLoading.set(false);
        },
        error: (err) => {
          this.errorMessage.set("Data could not be loaded. Please try again.");
          this.isLoading.set(false);
        }
      });
    }
  }

  private calculateStats(tasks: UserTask[]): void {
    this.stats.totalTasks = tasks.length;
    this.stats.pending = tasks.filter(t => t.status === UserTaskStatus.Pending).length;
    this.stats.inProgress = tasks.filter(t => t.status === UserTaskStatus.InProgress).length;
    this.stats.done = tasks.filter(t => t.status === UserTaskStatus.Done).length;
  }

  onPriorityFilterChange(newPriority: string) {
    this.selectedPriority = newPriority as UserTaskPriority;
    this.loadDashboardData(this.selectedStatus, this.selectedPriority);
  }

  onStatusFilterChange(newStatus: string) {
    this.selectedStatus = newStatus as UserTaskStatus;
    this.loadDashboardData(this.selectedStatus, this.selectedPriority);
  }

  navigateToAddNewTask(): void {
    this.router.navigate(['/tasks/new']);
  }

  navigateToRegisterNewUser(): void {
    this.router.navigate(['/users/new']);
  }

  onTaskStatusChange(task: UserTask, newStatus: string): void {
    if (task.status === newStatus) return;
    this.isLoading.set(true);
    this.userTaskService.updateTaskStatus(task.id, newStatus).subscribe({
      next: () => {
        task.status = newStatus as UserTaskStatus;
        this.isLoading.set(false);
        this.loadDashboardData(this.selectedStatus);
      },
      error: (error) => {
        let backendMsg = 'Could not update task status.';
        if (error?.error?.message) {
          backendMsg = error.error.message;
        }
        this.errorMessage.set(backendMsg);
        this.isLoading.set(false);
      }
    });
  }
}
