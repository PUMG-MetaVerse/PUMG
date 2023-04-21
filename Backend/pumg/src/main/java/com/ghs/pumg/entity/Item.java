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
@Table(name = "item")
@DynamicInsert
@ToString
public class Item extends BaseEntity {

    @Column(name = "item_description")
    private String itemDescription;
    @Column(name = "hint")
    private String hint;

    public void addReview(long star) {

    }

}
