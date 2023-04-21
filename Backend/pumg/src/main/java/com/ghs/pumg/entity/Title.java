package com.ghs.pumg.entity;

import com.fasterxml.jackson.annotation.JsonIgnore;
import com.fasterxml.jackson.annotation.JsonProperty;
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
@Table(name = "title")
@DynamicInsert
@ToString
public class Title extends BaseEntity {

    @Column(name="title")
    private String title;

    @Column(name="description")
    private String description;

    public void addReview(long star) {

    }

}
