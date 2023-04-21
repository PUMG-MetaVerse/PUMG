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
@Table(name = "record_roomescape")
@DynamicInsert
@ToString
public class RecordRoomescape extends BaseEntity {

    @ManyToOne(fetch = FetchType.LAZY)
    @JoinColumn(name = "player_one_idx")
    private User playerOneIdx;
    @ManyToOne(fetch = FetchType.LAZY)
    @JoinColumn(name = "player_two_idx")
    private User playerTwoIdx;
    @ManyToOne(fetch = FetchType.LAZY)
    @JoinColumn(name = "player_three_idx")
    private User playerThreeIdx;
    @ManyToOne(fetch = FetchType.LAZY)
    @JoinColumn(name = "player_four_idx")
    private User playerFourIdx;

    @Column(name = "clear_time")
    private Time clearTime;

    public void addReview(long star) {

    }

}
