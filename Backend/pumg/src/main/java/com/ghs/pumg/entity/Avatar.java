package com.ghs.pumg.entity;

import lombok.*;
import org.hibernate.annotations.DynamicInsert;

import javax.persistence.*;

@Builder
@Getter
@Entity
@AllArgsConstructor
@NoArgsConstructor(access = AccessLevel.PROTECTED)
@Table(name = "avatar")
@DynamicInsert
@ToString
public class Avatar extends BaseEntity {

    @ManyToOne(fetch = FetchType.LAZY)
    @JoinColumn(name="map_idx")
    private Map mapIdx;

    public void addReview(long star) {

    }

}
