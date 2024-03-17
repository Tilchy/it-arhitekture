package org.tratnjek.trackmanagementsystem;

import io.quarkus.hibernate.orm.panache.PanacheEntity;
import jakarta.persistence.Entity;
import jakarta.persistence.GeneratedValue;
import jakarta.persistence.Id;
import jakarta.validation.constraints.*;

@Entity
public class Task extends PanacheEntity {
    @NotBlank(message = "Title of tha task is mandatory!")
    public String name;
    @NotBlank(message = "Description of the task is mandatory!")
    public String description;
    @NotBlank(message = "Status of the task is mandatory!")
    public String status;
    @NotNull(message = "Priority of the task is mandatory!")
    @Min(value = 1, message = "Priority should be between 1 and 5")
    @Max(value = 5, message = "Priority should be between 1 and 5")
    public int priority;
    @NotBlank(message = "Category of the task is mandatory!")
    public String category;
}
