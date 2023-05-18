package com.ghs.pumg.entity;


import lombok.*;
import org.hibernate.annotations.DynamicInsert;

import javax.persistence.*;
import java.sql.Time;

@Builder
@Getter
@Entity
@AllArgsConstructor
@NoArgsConstructor(access = AccessLevel.PROTECTED)
@Table(name = "ranking")
@DynamicInsert
@ToString
public class Ranking extends BaseEntity {

    @ManyToOne(fetch = FetchType.LAZY)
    @JoinColumn(name = "user_idx")
    private User userIdx;
    @Column(name = "score")
    private int score;
    @Column(name = "clear_time")
    private String clearTime;

    public void modifyScore(int score) {
        this.score = score;
    }

}
