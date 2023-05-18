package com.ghs.pumg.entity;

import lombok.Getter;
import lombok.Setter;
import lombok.ToString;
import org.springframework.format.annotation.DateTimeFormat;

import javax.persistence.*;
import java.time.LocalDateTime;

/**
 * 모델 간 공통 사항 정의.
 */
@Getter
@Setter
@MappedSuperclass
@ToString
public class BaseEntity {
    @Id
    @GeneratedValue(strategy = GenerationType.IDENTITY)
    Long idx;

    @Column(name = "created_at")
    @DateTimeFormat(pattern = "yyyy-MM-dd-HH-mm")
    private LocalDateTime createdAt = LocalDateTime.now();

    @Column(name = "updated_at")
    @DateTimeFormat(pattern = "yyyy-MM-dd-HH-mm")
    private LocalDateTime updatedAt = LocalDateTime.now();
}
