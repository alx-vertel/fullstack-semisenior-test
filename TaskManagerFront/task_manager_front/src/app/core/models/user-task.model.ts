import { UserTaskPriority } from "./enums/user-task-priority.enum";
import { UserTaskStatus } from "./enums/user-task-status.enum";

export interface AdditionalInfo {
  priority: UserTaskPriority;
  estimatedEndDate?: Date;
  tags: string[];
}

export interface UserTask {
  id: number;
  title: string;
  description?: string;
  status: UserTaskStatus;
  createdAt: Date;
  userId: number;
  userName?: string;
  additionalInfo: AdditionalInfo;
}