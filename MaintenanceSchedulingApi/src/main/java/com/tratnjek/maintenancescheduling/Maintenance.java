package com.tratnjek.maintenancescheduling;

import io.quarkus.hibernate.reactive.panache.PanacheEntity;
import jakarta.persistence.Entity;

import java.time.LocalDateTime;

@Entity
public class Maintenance extends PanacheEntity {

    public String name;
    public String description;
    public String location;
    public String duration;
    public String status;
    public String type;
    public String priority;
    public String notes;
}
