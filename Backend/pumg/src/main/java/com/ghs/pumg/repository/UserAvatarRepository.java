package com.ghs.pumg.repository;

import com.ghs.pumg.entity.User;
import com.ghs.pumg.entity.UserAvatar;
import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.stereotype.Repository;

import java.util.List;
import java.util.Optional;

@Repository
public interface UserAvatarRepository extends JpaRepository<UserAvatar, Long> {

//    Optional<User> findByIdAndPassword(String name, String password);
//    Optional<UserAvatar> findUserById(String userId);
    List<UserAvatar> findUserAvatarByUserIdx(User user);
}
