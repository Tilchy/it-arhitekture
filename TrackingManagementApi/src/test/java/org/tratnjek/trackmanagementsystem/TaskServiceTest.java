package org.tratnjek.trackmanagementsystem;

import io.quarkus.agroal.DataSource;
import io.quarkus.grpc.GrpcClient;
import io.quarkus.test.junit.QuarkusTest;
import io.quarkus.test.junit.TestProfile;
import jakarta.transaction.Transactional;
import org.junit.jupiter.api.Test;

import java.time.Duration;

import static org.junit.jupiter.api.Assertions.*;

@QuarkusTest
class TaskServiceTest {

    @GrpcClient
    TaskGrpcService taskService;

    @Test
    void testAddTask() {
        AddTaskResponse response = taskService
                .addTask(AddTaskRequest.newBuilder()
                        .setName("Task 1")
                        .setDescription("Description of task 1")
                        .setStatus("Open")
                        .setPriority(3)
                        .setCategory("Work")
                        .build())
                .await().atMost(Duration.ofSeconds(5));
        assertTrue(response.getSuccess());
    }

    @Test
    void testAddTaskWithInvalidPriority() {
        AddTaskResponse response = taskService
                .addTask(AddTaskRequest.newBuilder()
                        .setName("Task 2")
                        .setDescription("Description of task 2")
                        .setStatus("Open")
                        .setPriority(6)
                        .setCategory("Work")
                        .build())
                .await().atMost(Duration.ofSeconds(5));
        assertFalse(response.getSuccess());
        assertEquals("priority Priority should be between 1 and 5", response.getMessage());
    }

    @Test
    void testAddTaskWithInvalidName() {
        AddTaskResponse response = taskService
                .addTask(AddTaskRequest.newBuilder()
                        .setName("")
                        .setDescription("Description of task 3")
                        .setStatus("Open")
                        .setPriority(3)
                        .setCategory("Work")
                        .build())
                .await().atMost(Duration.ofSeconds(5));
        assertFalse(response.getSuccess());
        assertEquals("name Title of tha task is mandatory!", response.getMessage());
    }

    /*@Transactional
    @Test
    void testGetTask() {


        // delete all tasks
        Task.deleteAll();

        Task task = new Task();

        task.name = "Task 4";
        task.description = "Description of task 4";
        task.status = "Open";
        task.priority = 3;
        task.category = "Work";

        task.persistAndFlush();

        GetTaskResponse getTaskResponse = taskService
                .getTask(GetTaskRequest.newBuilder()
                        .setId(task.id.toString())
                        .build())
                .await().atMost(Duration.ofSeconds(5));


        assertEquals("Task 4", getTaskResponse.getName());
    }*/
}