package com.ghs.pumg.repository;

import com.ghs.pumg.entity.Avatar;
import com.ghs.pumg.entity.User;
import org.springframework.data.jpa.repository.JpaRepository;
import org.springframework.stereotype.Repository;

import java.util.Optional;

@Repository
public interface AvatarRepository extends JpaRepository<Avatar, Long> {

//    Optional<Avatar> findByIdAndPassword(String name, String password);
//    Optional<Avatar> findUserById(String userId);
}
