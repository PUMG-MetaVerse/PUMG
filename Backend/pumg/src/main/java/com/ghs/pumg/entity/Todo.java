package com.ghs.pumg.entity;


import lombok.*;
import org.hibernate.annotations.DynamicInsert;

import javax.persistence.*;
import java.sql.Time;
import java.time.LocalDateTime;

@Builder
@Getter
@Entity
@AllArgsConstructor
@NoArgsConstructor(access = AccessLevel.PROTECTED)
@Table(name = "todo")
@DynamicInsert
@ToString
public class Todo extends BaseEntity {

    @ManyToOne(fetch = FetchType.LAZY)
    @JoinColumn(name = "user_idx")
    private User userIdx;
    @Column(name = "content")
    private String content;
    @Column(name = "end_time")
    private LocalDateTime endTime;

    public void addReview(long star) {

    }

}
