package com.tratnjek.maintenancescheduling;

import io.quarkus.test.junit.QuarkusTest;
import org.junit.jupiter.api.BeforeEach;
import org.junit.jupiter.api.Test;

import static io.restassured.RestAssured.given;
import static org.hamcrest.Matchers.is;

@QuarkusTest
public class MaintenanceResourceTest {

    public long createdId = -1;

    @Test
    void testListMaintenanceTasks(){
        given()
                .when().get("/maintenance")
                .then()
                .statusCode(200)
                .body(is("[]"));
    }

    @Test
    void testGetMaintenanceTask() {
        given()
                .when().get("/maintenance/{id}", createdId) // Assuming the created task has ID 1
                .then()
                .statusCode(200)
                .body("name", is("Test maintenance"))
                .body("status", is("Test status"));
    }

    @Test
    void testAddMaintenanceTask() {
        Maintenance maintenance = new Maintenance();
        maintenance.name = "New maintenance";
        maintenance.status = "New status";
        maintenance.description = "New description";
        maintenance.location = "New location";
        maintenance.duration = "New duration";
        maintenance.type = "New type";
        maintenance.priority = "New priority";
        maintenance.notes = "New notes";

        given()
                .when().body(maintenance).post("/maintenance")
                .then()
                .statusCode(200)
                .body("name", is("New maintenance"))
                .body("status", is("New status"));
    }

}
