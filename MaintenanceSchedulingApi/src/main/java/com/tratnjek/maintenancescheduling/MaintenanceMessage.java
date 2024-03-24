package com.tratnjek.maintenancescheduling;

import io.quarkus.runtime.annotations.RegisterForReflection;

@RegisterForReflection
public class MaintenanceMessage {
    public String name;
    public String status;
}
