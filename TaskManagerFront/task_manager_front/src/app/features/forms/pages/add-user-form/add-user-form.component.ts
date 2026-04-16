import { Component, inject, signal } from '@angular/core';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { Router, RouterModule } from '@angular/router';
import { UserService } from '../../../../core/models/services/user.service';
import { CreateUserDto } from '../../../../core/models/dtos/user/create-user-dto';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'app-add-user-form.component',
  imports: [ReactiveFormsModule, RouterModule, CommonModule],
  templateUrl: './add-user-form.component.html',
  styleUrl: './add-user-form.component.css',
})
export class AddUserFormComponent {
  private fb = inject(FormBuilder);
  private userService = inject(UserService);
  private router = inject(Router);

  isSubmitting = signal(false);

  userForm = this.fb.group({
    name: ['', [Validators.required, Validators.minLength(3)]],
    email: ['', [Validators.required, Validators.email]]
  });

  onSubmit() {
    if (this.userForm.invalid) return;

    const dto: CreateUserDto = {
      name: this.userForm.value.name ?? '',
      email: this.userForm.value.email ?? ''
    };

    this.isSubmitting.set(true);
    this.userService.createUser(dto).subscribe({
      next: () => this.router.navigate(['/']),
      error: () => this.isSubmitting.set(false)
    });
  }
}
