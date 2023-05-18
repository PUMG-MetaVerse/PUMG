package com.ghs.pumg.entity;

import com.fasterxml.jackson.annotation.JsonIgnore;
import com.fasterxml.jackson.annotation.JsonProperty;
import lombok.*;
import org.hibernate.annotations.DynamicInsert;
import javax.persistence.*;

@Builder
@Getter
@Entity
@AllArgsConstructor
@NoArgsConstructor(access = AccessLevel.PROTECTED)
@Table(name = "user")
@DynamicInsert
@ToString
public class User extends BaseEntity {

    @Column(unique = true, length = 50)
    private String id;

    @JsonIgnore
    @JsonProperty(access = JsonProperty.Access.WRITE_ONLY)
    private String password;

    private String nickName;

    @Column(name = "fish_cnt")
    private int fishCnt;

    @Column(name = "camp_cnt")
    private int campCnt;

    @Column(name = "party_cnt")
    private int partyCnt;
    @Column(name = "title")
    private String title;

    public void modifyNickname(String nickName) {
        this.nickName = nickName;
    }
    public void modifyTitle(String title) {
        this.title = title;
    }


}
