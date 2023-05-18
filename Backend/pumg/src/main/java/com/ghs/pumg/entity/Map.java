package com.ghs.pumg.entity;

import lombok.*;
import org.hibernate.annotations.DynamicInsert;

import javax.persistence.Column;
import javax.persistence.Entity;
import javax.persistence.Table;

@Builder
@Getter
@Entity
@AllArgsConstructor
@NoArgsConstructor(access = AccessLevel.PROTECTED)
@Table(name = "map")
@DynamicInsert
@ToString
public class Map extends BaseEntity {

    @Column(name="map_name")
    private String mapName;

    @Column(name="map_info")
    private String mapInfo;

    public void addReview(long star) {

    }

}
