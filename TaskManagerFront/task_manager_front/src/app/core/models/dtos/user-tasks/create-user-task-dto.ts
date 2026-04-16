import { UserTaskPriority } from "../../enums/user-task-priority.enum";

export interface CreateUserTaskDto {
  title: string;
  description?: string | null;
  userId: number;
  additionalInfo?: AdditionalInfoDto | null;
}

export interface AdditionalInfoDto {
    priority?: UserTaskPriority | null;
    estimatedEndDate?: string | null;
    tags?: string[] | null;
}